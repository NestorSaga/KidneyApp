const mongoose = require('mongoose');
const Achievment = mongoose.model('achievements')
const UserAchievment = mongoose.model('userAchievements')
const res = require('express/lib/response');
const { debug } = require('console');


module.exports = app => {

    app.post('/account/addAchievment', async (request, response) => {

        var res = {};

        const {rUserId, rAchievmentName, rCompletion} = request.body;

        //var userId = await Account.findOne({_id : rUserId}, '_id');
        var foundAchievmentId = await Achievment.findOne({name : rAchievmentName}, 'name')


        if(foundAchievmentId != null){

            var newUserAchievment = new UserAchievment({
                userId : rUserId,
                achievementId : foundAchievmentId._id,
                completion: rCompletion,

                date : Date.now()
            });

            await newUserAchievment.save();

            res.code = 0;
            res.msg = "Achievment added";           
            response.send(res);
            return;

            
        } else {
            res.code = 1;
            res.msg = "Achievment doesnt exist";
            response.send(res);
            return;
        }
    });


}