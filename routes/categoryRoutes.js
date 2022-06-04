const mongoose = require('mongoose');
const Account = mongoose.model('users');
const Achievment = mongoose.model('achievements')
const UserAchievment = mongoose.model('userAchievements')
const VideoRegistry = mongoose.model('videoRegistry')
const Category = mongoose.model('categories');
const res = require('express/lib/response');
const { debug, Console } = require('console');


module.exports = app => {

    app.post('/categories/names', async(request, response) => {

        var res = {};

        var names = [];

        Category.find({}, function(err, result) {
            if (err) throw err;

            result.forEach(element => {
                names = [...names, element.name];
            });

            res.code = 0;
            res.msg = "Categories obtained";
            res.names = names; 
    
            response.send(res);

        });

        return;

    });

}