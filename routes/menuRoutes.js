const mongoose = require('mongoose');
const Account = mongoose.model('users');
const Food = mongoose.model('foods');
const res = require('express/lib/response');
const { debug, Console } = require('console');


module.exports = app => {

    app.post('/account/updateClientFood', async(request, response) => {


        var updateFood = await Food.find();
      
        console.log(updateFood);
       
        response.send(updateFood);

        return;

    });

    

}