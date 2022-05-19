const mongoose = require('mongoose');
const Quiz = mongoose.model('quizzes')
const UserAchievment = mongoose.model('userAchievements')
const userQuiz = mongoose.model('userQuizzes');
const Category = mongoose.model('categories');
const res = require('express/lib/response');
const { debug, Console } = require('console');


module.exports = app => {

    app.post('/quiz/getQuizzesFromCategoryName', async(request, response) => {

        console.log("Attempting to obtain quizzes from category name");

        var res = {};

        const {rCategory} = request.body;

        var categories = await Category.findOne({name: rCategory}, '_id');

        var quizzes = await Quiz.find({categoryId: categories._id});

        console.log("Quizzes");
        console.log(quizzes);

        response.send(quizzes);

        return;

    });

}