const mongoose = require('mongoose');
const Account = mongoose.model('users');
const Achievment = mongoose.model('achievements')
const UserAchievment = mongoose.model('userAchievements')
const res = require('express/lib/response');
const { debug, Console } = require('console');


module.exports = app => {

    app.post('/account/retrieveData', async (request, response) => {


        var res = {};

        const {rUserId} = request.body;

        //console.log(rUserId);
        
        var data = await UserAchievment.find({userId : rUserId}, 'userId achievementId date completion');

        console.log(data);

        res._id = rUserId;
        var stringified = JSON.stringify(data);
        var parsed = JSON.parse(stringified)
        res.userAchievements = parsed
        
        console.log(stringified);
       
        response.send(res);

        console.log(res);

        return;

    });

}