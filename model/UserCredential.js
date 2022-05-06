const mongoose = require('mongoose');
const {Schema} = mongoose;

const userCredentialSchema = new Schema({ 
    
    userId: String,
    key: String,
    salt: String,
    durationDays: Number,
    validationDate: Date,
});

mongoose.model('userCredentials', userCredentialSchema);