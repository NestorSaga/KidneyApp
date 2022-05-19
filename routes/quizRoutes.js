const mongoose = require('mongoose');
const Quiz = mongoose.model('quizzes')
const Question = mongoose.model('questions');
const UserAchievment = mongoose.model('userAchievements')
const userQuiz = mongoose.model('userQuizzes');
const Category = mongoose.model('categories');
const res = require('express/lib/response');
const { debug, Console } = require('console');


module.exports = app => {

    app.post('/quiz/getQuizzesFromCategoryName', async(request, response) => {

        const {rCategory} = request.body;

        var categories = await Category.findOne({name: rCategory}, '_id');

        var quizzes = await Quiz.find({categoryId: categories._id});

        response.send(quizzes);

        return;

    });

    app.post('/quiz/getQuestionsFromQuiz', async(request, response) => {

        const {rQuizId} = request.body;

        var questions = await Question.find({quizId: rQuizId});

        console.log(questions);
        response.send(questions);

        return;

    });

}