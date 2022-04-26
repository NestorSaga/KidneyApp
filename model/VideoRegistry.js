const mongoose = require('mongoose');
const {Schema} = mongoose;

const videoRegistrySchema = new Schema({ 
    
    userId: String,
    videoId: String,
    rating: Number,
    date: Date
});

mongoose.model('videoRegistry', videoRegistrySchema);