const mongoose = require('mongoose');
const Quiz = mongoose.model('quizzes')
const Question = mongoose.model('questions');
const UserQuiz = mongoose.model('userQuizzes');
const Category = mongoose.model('categories');
const res = require('express/lib/response');
const { debug, Console, time } = require('console');


module.exports = app => {

    app.post('/quiz/getQuizzesFromCategoryName', async(request, response) => {

        const { rCategory } = request.body;

        var categories = await Category.findOne({ name: rCategory }, '_id');

        var quizzes = await Quiz.find({ categoryId: categories._id });

        response.send(quizzes);

        return;

    });

    app.post('/quiz/getQuestionsFromQuiz', async(request, response) => {

        const { rQuizId } = request.body;

        var questions = await Question.find({ quizId: rQuizId });

        response.send(questions);

        return;

    });

    app.post('/quiz/saveQuiz', async(request, response) => {

        var res = {};

        const { rScore, rUserId, rQuizId } = request.body;

        userQuiz = new UserQuiz({
            userId: rUserId,
            quizId: rQuizId,
            score: rScore,
            date: Date.now()
        });

        await userQuiz.save();

        res.code = 0;

        response.send(res);

        return;

    });

    app.post('/quiz/getHighscore', async(request, response) => {

        var res = {};

        const { rUserId, rQuizId } = request.body;

        var userScore = await UserQuiz.findOne({ userId: rUserId, quizId: rQuizId }, 'score').sort({ score: -1 });

        if (userScore != null) {
            res.code = 0;
            res.score = userScore.score;
        } else {
            res.code = 1;
            res.score = 0;
        }

        response.send(res);

        return;

    });

}