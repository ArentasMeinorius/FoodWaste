/// <reference types="cypress" />

import { forEach } from "cypress/types/lodash";

describe('FoodWaste allergens', () => {
    beforeEach(() => {
      cy.visit('https://localhost:44368');
      cy.get('a').contains('Login').click();
      cy.get('#Input_UserName').type('TestAccount');
      cy.get('#Input_Password').type('q!W2E3R4T5');
      cy.get('button').contains('Log in').click();
      cy.get('.navbar-brand').click();
    })

    afterEach(() => {
      cy.get('button:contains("Logout")').click();
    })
  
    it('Creates product, user is allergic to the product', () => {
      //product creation stage
      let allergenNames = ['Milk', 'Eggs', 'Peanuts']
      let productName = '1111111';
      cy.get('.nav-link').contains('Products').click()
      cy.get('a').contains('Add a product').click()
   
      for(var i=1;i<=allergenNames.length;i++){
        CreateNewAllergen(allergenNames[i-1],i%3!=0);
      }

      cy.get('#Name').type(productName);
      cy.get('input').contains('Create').click();

      //product check stage
      cy.get('.nav-link').contains('Products').click();
      cy.get('tr').children('td').contains(productName).parent().children('td').contains('Details').click();
      for (var i=0;i<allergenNames.length-1;i++){
        cy.get(`div:contains("${allergenNames[i]}")`).children('a:contains("I\'m allergic to this")').should('have.length', 1);
      }
      cy.get('div').children('a:contains("This is dangerous")').should('have.length', 0);

      cy.get('a:contains("My Allergens")').click();
      cy.get('matlistItem').children('div').contains(allergenNames[0]).parent().children().children('matbutton').contains('Add').click();

      cy.get('.nav-link').contains('Products').click();
      cy.get('tr').children('td').contains(productName).parent().children('td').contains('Details').click();
      for (var i=1;i<allergenNames.length-1;i++){
        cy.get(`div:contains("${allergenNames[i]}")`).children('a:contains("I\'m allergic to this")').should('have.length', 1);
      }
      cy.get('div').children('a:contains("This is dangerous")').should('have.length', 1);
      cy.get(`div:contains("${allergenNames[0]}")`).children('a:contains("This is dangerous")').should('have.length', 1);

      cy.get('a:contains("My Allergens")').click();
      cy.get('matlistItem').children('div').contains(allergenNames[0]).parent().children().children('matbutton').contains('Remove').click();

      DeleteProduct(productName);
    })

    it('Creates allergen, it appears in list, no empty fields', () => {
      var productName = "testCreate";
      cy.get('.nav-link').contains('Products').click();
      cy.get('a').contains('Add a product').click();

      CreateNewAllergen('CreatedAllergen',true);
      cy.get('#Name').type(productName);
      cy.get('input').contains('Create').click();

      DeleteProduct(productName);
    })

    it('Removes allergen, one should be removed, not all of them', () => {
      var productName = "22222222";
      cy.get('.nav-link').contains('Products').click()
      cy.get('a').contains('Add a product').click()

      var allergenNames = ['Milk', 'Eggs', 'Peanuts'];
      for(var i=0;i<allergenNames.length;i++){
        CreateNewAllergen(allergenNames[i],true);
      }
      for(var i=0;i<allergenNames.length;i++){
        cy.get('matlistItem').children('div').contains(allergenNames[i]).parent().children().children('matbutton').contains('Remove').should('have.length', 1);
        cy.get('matlistItem').children('div').contains(allergenNames[i]).parent().children().children('matbutton').contains('Add').should('have.length', 0);
      }

      cy.get('matlistItem').children('div').contains(allergenNames[0]).parent().children().children('matbutton').contains('Remove').click();
      
      cy.get('matbutton:contains("Remove")').should('have.length', allergenNames.length-1);
      for(var i=1;i<allergenNames.length;i++){
        cy.get('matlistItem').children('div').contains(allergenNames[i]).parent().children().children('matbutton').contains('Remove').should('have.length', 1);
        cy.get('matlistItem').children('div').contains(allergenNames[i]).parent().children().children('matbutton').contains('Add').should('have.length', 0);
      }

      cy.get('matlistItem').children('div').contains(allergenNames[0]).parent().children().children('matbutton').contains('Add').should('have.length', 1);
      
      cy.get('#Name').type(productName);
      cy.get('input').contains('Create').click();

      DeleteProduct(productName);
    })

    it('Create allergen, different allergens should appear', () => {
      var productName="111111";
      cy.get('.nav-link').contains('Products').click();
      cy.get('a').contains('Add a product').click();
      
      var allergenNames = ['Milk', 'Eggs', 'Peanuts'];
      for(var i=0;i<allergenNames.length;i++){
        CreateNewAllergen(allergenNames[i],true);
      }

      cy.get('#Name').type(productName);
      cy.get('input').contains('Create').click();

      cy.get('.nav-link').contains('Products').click();
      cy.get('tr').children('td').contains(productName).parent().children('td').contains('Details').click();

      for(var i=0;i<allergenNames.length;i++){
        cy.get(`div:contains("${allergenNames[i]}")`).children('a:contains("I\'m allergic to this")').should('have.length', 1);
      }
      
      DeleteProduct(productName);
    })

    function CreateNewAllergen(name:string, add : boolean){
      cy.get('#newAllergen').children('input').type(name).parent().children('matbutton').click();
      cy.get('matlistItem').children('div').contains(name).parent().children().children('matbutton').contains('Add').should('have.length', 1);
      if(add){
        cy.get('matlistItem').children('div').contains(name).parent().children().children('matbutton').contains('Add').click()
      }
    }

    function DeleteProduct(name:string){
      cy.get('.nav-link').contains('Products').click()
      cy.get('tr').children('td').contains(name).parent().children('td').contains('Delete').click()
      cy.get('input').contains('Delete').click()
    }
  })
  