const mongoose = require('mongoose');
const Account = mongoose.model('users');
const Achievment = mongoose.model('achievements')
const UserAchievment = mongoose.model('userAchievements')
const VideoRegistry = mongoose.model('videoRegistry')
const res = require('express/lib/response');
const { debug, Console } = require('console');


module.exports = app => {

    app.post('/account/retrieveData', async (request, response) => {

        var res = {};

        const {rUserId} = request.body;

        // fetch each table of user data

        res._id = rUserId;
        
        var userAchievmentData = await UserAchievment.find({userId : rUserId}, 'achievementId date completion');

        var stringified = JSON.stringify(userAchievmentData);
        var parsed = JSON.parse(stringified)
        res.userAchievements = parsed

        var videoRegistryData = await VideoRegistry.find({userId : rUserId}, 'videoId date rating');

        console.log("Video registry data: " + videoRegistryData)

        var stringified = JSON.stringify(videoRegistryData);
        var parsed = JSON.parse(stringified)
        res.seenVideos = parsed
        
        response.send(res);

        console.log(res);

        return;

    });

}