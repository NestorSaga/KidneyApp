const mongoose = require('mongoose');
const Account = mongoose.model('accounts');

module.exports = app => {

    // Routes
    app.get('/account', async (request, response) => {

    const {rUsername, rPassword} = request.query;

        if(rUsername == null || rPassword == null)
        {
            response.send("insvaslid credens");
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

        response.send("Invalid credents");
        return;
    });

}