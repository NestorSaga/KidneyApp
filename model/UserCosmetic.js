const mongoose = require('mongoose');
const {Schema} = mongoose;

const userCosmeticSchema = new Schema({
    userId: String,
    cosmetics: {
        hat: Number,
        face: Number,
        body: Number
    }
});

mongoose.model('userCosmetics', userCosmeticSchema);