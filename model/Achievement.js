const mongoose = require('mongoose');
const {Schema} = mongoose;

const achievementSchema = new Schema({ 
    
    name: String,

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