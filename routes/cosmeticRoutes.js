const mongoose = require('mongoose');
const UserCosmetic = mongoose.model('userCosmetics');
const res = require('express/lib/response');
const { debug, Console } = require('console');


module.exports = app => {

    app.post('/cosmetics/getUserCosmetics', async(request, response) => {

        var res = {};

        const { rUserId } = request.body;

        var userCosmetics = await UserCosmetic.findOne({ userId: rUserId }, 'cosmetics');

        if (userCosmetics == null) {
            
            userCosmetics = new UserCosmetic({
                userId: rUserId,
                cosmetics: {
                    hat: 0,
                    face: 0,
                    body: 0
                }
            });
    
            await userCosmetics.save();
        } 

        res.code = 0;
        res.message = "User cosmetics obtained";
        res.cosmetics = userCosmetics.cosmetics
        console.log(res);
        response.send(res);

        return;

    });

    app.post('/cosmetics/setUserCosmetics', async(request, response) => {

        var res = {};

        const { rUserId, rHat, rFace, rBody } = request.body;

        if(rUserId == null || rHat == null && rFace == null || rBody == null || parseInt(rHat) < 0 || parseInt(rFace) < 0 || parseInt(rBody) < 0)
        {
            res.code = 1;
            res.msg = "Invalid credentials";
            response.send(res);
            return;
        }

        var userCosmetics = await UserCosmetic.findOne({ userId: rUserId }, 'cosmetics');

        if (userCosmetics == null) {
            
            userCosmetics = new UserCosmetic({
                userId: rUserId,
                cosmetics: {
                    hat: 0,
                    face: 0,
                    body: 0
                }
            });
    
            await userCosmetics.save();
  
        } else {
            await UserCosmetic.updateOne(
                {userId: rUserId},
                {$set: {cosmetics: {
                    hat: parseInt(rHat),
                    face: parseInt(rFace),
                    body: parseInt(rBody)
                }}},
            );
        }

        res.code = 0;
        res.message = "User cosmetics updated";
        response.send(res);

        return;

    });

    app.post('/cosmetics/createUserCosmetics', async(request, response) => {

        var res = {};

        const { rUserId } = request.body;

        userCosmetics = new UserCosmetic({
            userId: rUserId,
            cosmetics: {
                hat: 0,
                face: 0,
                body: 0
            }
        });

        await userCosmetics.save();

        res.code = 0;
        res.message = "User cosmetics created";
        response.send(res);

        return;

    });

}