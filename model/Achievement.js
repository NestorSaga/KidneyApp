const mongoose = require('mongoose');
const {Schema} = mongoose;

const achievementSchema = new Schema({ 
    
    name: String,
    targetProgress: Number,

    displayName: {
        en: String,
        es: String
    },
    description: {
        en: String,
        es: String
    }
});

mongoose.model('achievements', achievementSchema);