const mongoose = require('mongoose');
const Account = mongoose.model('users');
const argon2 = require('argon2');
const crypto = require('crypto');
const res = require('express/lib/response');

const passwordRegex = new RegExp("(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{6,24})");

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

        var userAccount = await Account.findOne({username : rUsername}, 'username password');
        if(userAccount != null){

            argon2.verify(userAccount.password, rPassword).then(async (success) => {

                if(success) {
                    userAccount.lastAuth = Date.now();
                    await userAccount.save();

                    res.code = 0;
                    res.msg = "Account found";
                    res.data = ( ({username}) => ({username}) )(userAccount); // para enviar mas datos, ({data1, data2, etc}) => ({data1, data2, etc})

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

        const {rUsername, rPassword} = request.body;

        if(rUsername == null || rUsername < 3 || rUsername > 24)
        {
            res.code = 1;
            res.msg = "Invalid credentials";
            response.send(res);
            return;
        }

        if(passwordRegex.test(rPassword)) {
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
                        salt: salt,
        
                        lastAuth : Date.now()
                    });

                    await newAccount.save();

                    res.code = 0;
                    res.msg = "Account found";
                    res.data = ( ({username}) => ({username}) )(newAccount); // para enviar mas datos, ({data1, data2, etc}) => ({data1, data2, etc})

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

}