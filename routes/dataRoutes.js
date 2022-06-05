const mongoose = require('mongoose');
const UserAchievment = mongoose.model('userAchievements');
const VideoRegistry = mongoose.model('videoRegistry');
const UserCosmetic = mongoose.model('userCosmetics');
const Tip = mongoose.model('tips');
const res = require('express/lib/response');
const { debug, Console } = require('console');


module.exports = app => {

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