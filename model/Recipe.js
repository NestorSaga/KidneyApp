const mongoose = require('mongoose');
const {Schema} = mongoose;

const recipeSchema = new Schema({
    recipeId:String,
    name:String,
    author:String,
    ingredients:Object,
    description:String,
    score:Number,
    IMCValue:Number,

});

mongoose.model('recipes', recipeSchema);