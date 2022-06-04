const mongoose = require('mongoose');
const {Schema} = mongoose;

const quizSchema = new Schema({ 

    categoryId: String,
    name: String,

    displayName: {
        en: String,
        es: String
    },
    __v: Number
});

mongoose.model('quizzes', quizSchema);