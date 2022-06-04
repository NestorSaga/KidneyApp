const mongoose = require('mongoose');
const {Schema} = mongoose;

const userAchievementSchema = new Schema({ 
    
    userId: String,
    achievementId: String,
    progress: Number,
    completed:Boolean,
    date: Date
});

mongoose.model('userAchievements', userAchievementSchema);