const mongoose = require('mongoose');
const {Schema} = mongoose;

const userQuizSchema = new Schema({ 
    
    userId: String,
    quizId: String,
    score: Number,
    date: Date
});

mongoose.model('userQuizzes', userQuizSchema);