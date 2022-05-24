const mongoose = require('mongoose');
const UserAchievment = mongoose.model('userAchievements');
const VideoRegistry = mongoose.model('videoRegistry');
const Tip = mongoose.model('tips');
const res = require('express/lib/response');
const { debug, Console } = require('console');


module.exports = app => {

    app.post('/account/retrieveData', async(request, response) => {

        var res = {};

        const { rUserId, rUserName } = request.body;

        // fetch each table of user data

        res._id = rUserId;
        res.userName = rUserName;

        var userAchievmentData = await UserAchievment.find({ userId: rUserId }, 'achievementId date completion');

        var stringified = JSON.stringify(userAchievmentData);
        var parsed = JSON.parse(stringified)
        res.userAchievements = parsed

        var videoRegistryData = await VideoRegistry.find({ userId: rUserId }, 'videoId date rating');

        var stringified = JSON.stringify(videoRegistryData);
        var parsed = JSON.parse(stringified)
        res.seenVideos = parsed

        response.send(res);

        return;

    });

    app.post('/tip', async(request, response) => {

        var res = {};

        var tip = Tip.aggregate([{ $sample: { size: 1 } }, 'languageTip']);
        //var tip = Tip.find();

        res.code = 0;
        res.tip = tip;
        response.send(res);

        return;

    });

}