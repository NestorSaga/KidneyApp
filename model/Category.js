const mongoose = require('mongoose');
const {Schema} = mongoose;

const categorySchema = new Schema({ 
    
    name: String,

    displayName: {
        en: String,
        es: String
    },
    description: {
        en: String,
        es: String
    },

    __v: Number
});

mongoose.model('categories', categorySchema);