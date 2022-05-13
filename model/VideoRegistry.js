const mongoose = require('mongoose');
const {Schema} = mongoose;

const videoRegistrySchema = new Schema({ 
    
    userId: String,
    videoId: String,
    date: Date
});

mongoose.model('videoRegistry', videoRegistrySchema);