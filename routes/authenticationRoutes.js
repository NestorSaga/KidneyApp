const mongoose = require('mongoose');
const Account = mongoose.model('accounts');

module.exports = app => {

    app.post('/account/login', async (request, response) => {

        const {rUsername, rPassword} = request.body;

        if(rUsername == null || rPassword == null)
        {
            response.send("Invalid credentials (at least one null)");
            return;
        }

        var userAccount = await Account.findOne({username : rUsername});
        if(userAccount != null){

            if(rPassword == userAccount.password){ // NEEDS ENCRYPT
                userAccount.lastAuth = Date.now();
                await userAccount.save();
                
                console.log("Retrieving acc...");
                response.send(userAccount);
                return;
            }
        }

        response.send("Invalid credentials (at least one wrong)");
        return;
    });

    app.post('/account/create', async (request, response) => {

        const {rUsername, rPassword} = request.body;

        if(rUsername == null || rPassword == null)
        {
            response.send("Invalid credentials (at least one null)");
            return;
        }

        var userAccount = await Account.findOne({username : rUsername});
        if(userAccount == null){
            //create new acc
            console.log("Attempting to create new acc...")

            var newAccount = new Account({
                username : rUsername,
                password : rPassword,

                lastAuth : Date.now()
            });

            await newAccount.save();

            response.send(newAccount);
            return;

        } else{
            if(rPassword == userAccount.password){ // NEEDS ENCRYPT
                userAccount.lastAuth = Date.now();
                await userAccount.save();
                
                console.log("Retrieving acc...");
                response.send(userAccount);
                return;
            }
        }

        response.send("Invalid credentials");
        return;
    });

}