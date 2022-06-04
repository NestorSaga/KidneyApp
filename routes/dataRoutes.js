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

    app.post('/randomTip', async(request, response) => {

        var res = {};

        var tip = await Tip.find({}, 'languageTip');

        if (tip != null) {
            res.code = 0;
            var rand = Math.random() * tip.length;
            res.tip = tip.at(rand).languageTip;

        } else {
            res.code = 1;
        }

        response.send(res);

        return;

    });

}