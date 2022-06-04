const mongoose = require('mongoose');
const { Schema } = mongoose;

const tipSchema = new Schema({

    languageTip: {
        en: String,
        es: String
    },
    __v: Number
});

mongoose.model('tips', tipSchema);