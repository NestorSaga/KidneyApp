const mongoose = require('mongoose');
const {Schema} = mongoose;

const foodSchema = new Schema({
    foodId:String,
    name:String,
    category:String,
    values:Object

});

mongoose.model('foods', foodSchema);