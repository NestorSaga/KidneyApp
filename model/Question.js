const mongoose = require('mongoose');
const {Schema} = mongoose;

const questionSchema = new Schema({ 
    
    quizId: String,
    statement: String,

    answers: {
        1: String,
        2: String,
        3: String,
    },
    
    correctAnswer: Number,
    language: String

});

mongoose.model('questions', questionSchema);