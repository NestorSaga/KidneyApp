const mongoose = require('mongoose');
const Account = mongoose.model('users');
const Attribute = mongoose.model('attributes');
const argon2 = require('argon2');
const crypto = require('crypto');
const res = require('express/lib/response');
const { debug } = require('console');

const passwordRegex = new RegExp("(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{6,32})");

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

    app.post('/account/create', async (request, response) => {

        var res = {};

        const {rUsername, rPassword, rName, rSurname1, rSurname2, rBirthDate, rSex, rHeight, rWeight, rState, rAttributeName, rEmail, rPhone, rCompanion, rExpert, rCompanionAccess} = request.body;

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
                        attributes: [rAttributeName],
                        mail: rEmail,
                        phone: rPhone,
                        companion: rCompanion == "No, Im a companion",
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

}