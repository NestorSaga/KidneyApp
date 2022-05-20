const mongoose = require('mongoose');
const Account = mongoose.model('users');
const Food = mongoose.model('foods');
const res = require('express/lib/response');
const { debug, Console } = require('console');


module.exports = app => {

    app.post('/account/updateClientFood', async(request, response) => {

        var res = {};

        const { rName, rCategory, rValues } = request.body;



        var updateFood = await Food.find();

        var stringifiedFood = JSON.stringify(updateFood);

        var parsed = JSON.parse(stringifiedFood)
        
        console.log(updateFood);
        console.log(stringifiedFood);
        console.log(parsed);
        
        response.send(updateFood);

        return;

    });

}