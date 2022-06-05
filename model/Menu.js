const mongoose = require('mongoose');
const {Schema} = mongoose;

const menuSchema = new Schema({
    recipeId:String,
    name:String,
    author:String,
    aliments:Array,
    description:String,
    score:Number,
    IMCValue:Number

});

mongoose.model('menus', menuSchema);