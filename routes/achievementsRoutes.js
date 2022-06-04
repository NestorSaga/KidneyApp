const mongoose = require('mongoose');
const Achievment = mongoose.model('achievements')
const UserAchievment = mongoose.model('userAchievements')
const res = require('express/lib/response');
const { debug, Console } = require('console');


module.exports = app => {

    app.post('/account/addAchievment', async (request, response) => {

        var res = {};

        const {rUserId, rAchievmentName, rProgress, } = request.body;

        //var userId = await Account.findOne({_id : rUserId}, '_id');
        var foundAchievment = await Achievment.findOne({name : rAchievmentName}, 'name _id targetProgress')


        if(foundAchievment != null ){
            
            var foundUserAchievement = await UserAchievment.findOne({userId:rUserId, achievementId:foundAchievment._id});

            if(foundUserAchievement != null){

                if(foundUserAchievement.completed == false){
                    var prog = foundUserAchievement.progress  + parseInt(rProgress);

                    //if prog => progress -> achievement unlocked
                    if(prog >= foundAchievment.targetProgress){
                        console.log("alo");
                        await UserAchievment.updateOne(
                            {_id:foundUserAchievement._id},
                            {$set: {date: Date.now(), progress:prog, completed:true}},
                        );

                        res.code = 0;
                        res.msg = "Achievement saved and completed"
                        res.data = ( ({_id}) => ({_id}) )(foundAchievment); 
                    }else{
                        
                        await UserAchievment.updateOne(
                            {_id:foundUserAchievement._id},
                            {$set: {date: Date.now(), progress:prog}},
                        );

                        res.code = 1;
                        res.msg = "Achievement progress saved"
                        res.data = prog; 
                    }
                    
                } else{//else achievement is already completed
                    res.code = 2;
                    res.msg = "achievement is already completed";
                    
                }

            }else{//else Create new userAchievement

                var isComp = foundAchievment.targetProgress <= rProgress;

                var newUserAchievement = new UserAchievment({
                    userId: rUserId,
                    achievementId: foundAchievment._id,
                    completed:isComp,
                    progress:rProgress,
                    date: Date.now()
                });
        
                await newUserAchievement.save();

                res.code = 3;
                res.msg = "Achievement progress created";
                
    

            } 

        }else{//else achivement doesnt exist return nope
            res.code = 4;
            res.msg = "Achievement doesn't exist";
            }

        response.send(res);
        return;

    });
}

        
            