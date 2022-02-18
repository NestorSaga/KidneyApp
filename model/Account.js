const mongoose = require('mongoose');
const {Schema} = mongoose;

const accountSchema = new Schema({ // TODO añadir toda la informacion
    username: String,
    password: String,

    lastAuth: Date,
});

mongoose.model('accounts', accountSchema);