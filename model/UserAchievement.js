const mongoose = require('mongoose');
const {Schema} = mongoose;

const userAchievementSchema = new Schema({ 
    
    userId: String ,
    achievementId: String,
    completion: Number,
    date: Date
});

mongoose.model('userAchievements', userAchievementSchema);