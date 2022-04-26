const mongoose = require('mongoose');
const {Schema} = mongoose;

const videoSchema = new Schema({ 
    
    categoryId: String,
    name: String,

    displayName: {
        en: String,
        es: String
    },
    description: {
        en: String,
        es: String
    },
    url: String

});

mongoose.model('videos', videoSchema);