const mongoose = require('mongoose');
const Video = mongoose.model('videos');
const Category = mongoose.model('categories');
const res = require('express/lib/response');
const { debug, Console } = require('console');


module.exports = app => {

    app.post('/videos/getByCategory', async(request, response) => {

        var res = {};

        const { rCategory } = request.body;

        var categories = await Category.findOne({ name: rCategory }, '_id');

        var videos = await Video.find({ categoryId: categories._id });


        if (videos != null) {
            res.code = 0;
            res.videos = videos;
        } else {
            res.code = 1;
        }

        console.log(res);

        response.send(res);

        return;

    });

}