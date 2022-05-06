const mongoose = require('mongoose');
const Account = mongoose.model('users');
const Attribute = mongoose.model('attributes');
const UserCredentials = mongoose.model('userCredentials');
const argon2 = require('argon2');
const crypto = require('crypto');
const res = require('express/lib/response');
const { debug, Console } = require('console');

const passwordRegex = new RegExp("(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{6,32})");
const defaultKeyDuration = 30;

module.exports = app => {

    app.post('/account/login', async (request, response) => {

        var res = {};

        const {rUsername, rPassword} = request.body;

        if(rUsername == null || !passwordRegex.test(rPassword))
        {   
            res.code = 1;
            res.msg = "Invalid credentials";
            response.send(res);
            return;
        }

        var userAccount = await Account.findOne({username : rUsername}, 'username password _id');
        if(userAccount != null){

            argon2.verify(userAccount.password, rPassword).then(async (success) => {

                if(success) {
                    userAccount.lastAuth = Date.now();
                    await userAccount.save();

                    res.code = 0;
                    res.msg = "Account found";
                    res.data = ( ({_id, username}) => ({_id, username}) )(userAccount); // para enviar mas datos, ({data1, data2, etc}) => ({data1, data2, etc})

                    response.send(res);
                    return;

                } else {
                    res.code = 1;
                    res.msg = "Invalid credentials";
                    response.send(res);
                    return;
                }

            });
        } else {
            res.code = 1;
            res.msg = "Invalid credentials";
            response.send(res);
            return;
        }
    });

    app.post('/account/autologin', async (request, response) => {

        console.log("Attempting autologin");

        var res = {};

        const {rKey} = request.body;

        if(rKey == null)
        {   
            res.code = 1;
            res.msg = "Invalid credentials";
            response.send(res);
            return;
        }

        var userCredentials = await UserCredentials.findOne({_id : rKey}, 'validationDate durationDays');
        if(userCredentials != null){

            const targetDate = new Date();
            targetDate.setDate(userCredentials.validationDate.getDate() + userCredentials.durationDays);
            
            if(targetDate > Date.now()) {

                UserCredentials.updateOne(
                    {_id: userCredentials._id},
                    {$set: {validationDate: Date.now()}}
                );

                await userCredentials.save();

                res.code = 0;
                res.msg = "Login Successful";
                res.data = ( ({userId, _id}) => ({userId, _id}) )(userCredentials); 
                response.send(res);
                return;
            }
        } else {
            res.code = 1;
            res.msg = "Key not valid";
            response.send(res);
            return;
        }
        
    });

    app.post('/account/create', async (request, response) => {

        var res = {};

        const {rUsername, rPassword, rName, rSurname1, rSurname2, rBirthDate, rSex, rHeight, rWeight, rState, rAttributeNames, rEmail, rPhone, rCompanion, rExpert, rCompanionAccess} = request.body;

        var stringAttributes = String(rAttributeNames).split("|")
        
        if(rUsername == null || rUsername < 3 || rUsername > 24)
        {
            res.code = 1;
            res.msg = "Invalid credentials";
            response.send(res);
            return;
        }

        if(!passwordRegex.test(rPassword)) {
            res.code = 3;
            res.msg = "Unsafe password";
            response.send(res);
            return;
        }

        var userAccount = await Account.findOne({username : rUsername}, '_id');
        if(userAccount == null){
            //create new acc
            console.log("Attempting to create new acc...")

            crypto.randomBytes(32, function(err, salt) {
                if(err) {
                    console.log(err);
                }

                argon2.hash(rPassword, salt).then(async (hash) => {

                    var newAccount = new Account({
                        username : rUsername,
                        password : hash,
                        name: rName,
                        surname1: rSurname1,
                        surname2: rSurname2,
                        birthDate: rBirthDate,
                        sex: rSex,
                        height: rHeight,
                        weight: rWeight,
                        state: rState,
                        attributes: stringAttributes,
                        mail: rEmail,
                        phone: rPhone,
                        companion: rCompanion == "True",
                        expertPatient: rExpert == "Yes",
                        companionAccess: rCompanionAccess == "Yes",

                        salt: salt,
                        lastAuth : Date.now()
                    });

                    await newAccount.save();

                    res.code = 0;
                    res.msg = "Account created";
                    res.data = ( ({username, _id}) => ({username, _id}) )(newAccount); // para enviar mas datos, ({data1, data2, etc}) => ({data1, data2, etc})

                    response.send(res);
                    return;
                    
                });
            });

        } else{
            res.code = 2;
            res.msg = "Username already taken";
            response.send(res);
        }
        return;
    });

    app.post('/account/addAttribute', async( request, response) => {

        var res = {};

        const {rName, rUserId} = request.body;

        if(rName == null)
        {
            res.code = 1;
            res.msg = "Invalid name";
            response.send(res);
            return;
        }

        if(rUserId == null)
        {
            res.code = 1;
            res.msg = "Invalid user id";
            response.send(res);
            return;
        }

        var attribute = await Attribute.findOne({name: rName});
        if(attribute == null) {
            res.code = 2;
            res.msg = "Attribute doesn't exist";
            response.send(res);
        }

        try{await Account.findOneAndUpdate(
            {_id:rUserId},
            {$addToSet: {attributes: attribute._id}}
        );

        
        }catch{
            res.code = 3;
            res.msg = "Account doesn't exist";
            response.send(res);
            return;
        }

        res.code = 0;
        res.msg = "Attribute added";
        res.data = attribute; // para enviar mas datos, ({data1, data2, etc}) => ({data1, data2, etc})

        response.send(res);
        return;
    });

    app.post('/account/getkey', async (request, response) => {

        console.log("Generating api key");

        var res = {};

        const {rUserId} = request.body;

        if(rUserId == null)
        {
            res.code = 1;
            res.msg = "Invalid credentials";
            response.send(res);
            return;
        }

        res.code = 1;
        res.msg = "Api key not obtained";

        var userCredentials = await UserCredentials.findOne({userId : rUserId}, 'userId validationDate durationDays _id');
        if(userCredentials != null ){

            const targetDate = new Date();
            targetDate.setDate(userCredentials.validationDate.getDate() + userCredentials.durationDays);
            
            if(targetDate > Date.now()) { //update key date

                console.log("Updating existing");

                UserCredentials.updateOne(
                    {_id: userCredentials._id},
                    {$set: {validationDate: Date.now()}}
                );

                await userCredentials.save();

                res.code = 0;
                res.msg = "Api key obtained";
                res.data = ( ({userId, _id}) => ({userId, _id}) )(userCredentials); 

            } else { // delete and generate new key
                console.log("Deleting and generating anew");

                UserCredentials.deleteOne({_id: userCredentials._id});

                userCredentials = new UserCredentials({
                    userId: rUserId,
                    durationDays: defaultKeyDuration,
                    validationDate: Date.now()
                });
        
                await userCredentials.save();
    
                res.code = 0;
                res.msg = "Api key obtained";
                res.data = ( ({userId, _id}) => ({userId, _id}) )(userCredentials); 
            }
            
            res.code = 0;
            res.msg = "Api key obtained";
            res.data = ( ({userId, _id}) => ({userId, _id}) )(userCredentials); // user and it's key (id)

        } else {

            userCredentials = new UserCredentials({
                userId: rUserId,
                durationDays: defaultKeyDuration,
                validationDate: Date.now()
            });
    
            await userCredentials.save();

            res.code = 0;
            res.msg = "Api key obtained";
            res.data = ( ({userId, _id}) => ({userId, _id}) )(userCredentials); 
        }

        console.log(res);

        response.send(res);
        return;

    });

}