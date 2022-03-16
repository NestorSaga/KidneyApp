const mongoose = require('mongoose');
const {Schema} = mongoose;

const userSchema = new Schema({ // TODO añadir toda la informacion
    username: String,
    password: String,
    salt: String,
    attributes: [String],

    lastAuth: Date,
});

mongoose.model('users', userSchema);