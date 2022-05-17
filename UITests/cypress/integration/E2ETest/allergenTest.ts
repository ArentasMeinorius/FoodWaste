/// <reference types="cypress" />

describe('FoodWaste allergens', () => {// add create your own products and then use everywhere,delete when unused
    beforeEach(() => {
      cy.visit('https://localhost:44368')
      cy.get('a').contains('Login').click()
      cy.get('#Input_UserName').type('TestAccount')
      cy.get('#Input_Password').type('q!W2E3R4T5')
      cy.get('button').contains('Log in').click()
      //check if user has allergens
      cy.get('a').contains('My Allergens').click()
      //iteratively remove allergens by clicking remove button
      cy.log(Cypress.$("matbutton#RemoveAllergen.btn.btn-primary").length.toString());
      // while(Cypress.$.find("matbutton#RemoveAllergen.btn.btn-primary").length > 0){// somewhy it does not work: \ 
      //   cy.get('matbutton').contains('Remove').click()
      // }
      
      // cy.get('matbutton:contains("Remove")').should('have.length', 0)

      cy.get('.navbar-brand').click()
    })

    afterEach(() => {
      cy.get('button:contains("Logout")').click()
    })
  
    it('Creates product, user is allergic to the product', () => {
      // createProduct
      cy.get('.nav-link').contains('Products').click()
      cy.get('a').contains('Add a product').click()
      // later add create product then use them
      cy.get('matbutton').contains('Add').click()
      cy.get('#Name').type('testProduct')
      cy.get('input').contains('Create').click()

      cy.get('.nav-link').contains('Products').click()
      cy.get('tr').children('td').contains('testProduct').parent().children('td').contains('Details').click()
      cy.get('div:contains("Milk")').children('a:contains("I\'m allergic to this")').should('have.length.greaterThan', 0)
      cy.get('div').children('a:contains("This is dangerous")').should('have.length', 0)

      cy.get('a:contains("My Allergens")').click()
      cy.get('matlistItem').children('div').contains('Milk').parent().children().children('matbutton').contains('Add').click()

      cy.get('.nav-link').contains('Products').click()
      cy.get('tr').children('td').contains('testProduct').parent().children('td').contains('Details').click()
      cy.get('div:contains("Milk")').children('a:contains("This is dangerous")').should('have.length.greaterThan', 0)

      cy.get('a:contains("My Allergens")').click()
      cy.get('matlistItem').children('div').contains('Milk').parent().children().children('matbutton').contains('Remove').click()// remove milk and more

      // while(Cypress.$("#RemoveAllergen").length > 0){// somewhy it does not work
      //   cy.get('matbutton').contains('Remove').click()
      // }
      // cy.get('matbutton:contains("Remove")').should('have.length', 0)


      cy.get('.nav-link').contains('Products').click()
      cy.get('tr').children('td').contains('testProduct').parent().children('td').contains('Delete').click()
      cy.get('input').contains('Delete').click()
    })

    it('Creates allergen, it appears in list, no empty fields', () => {
      cy.get('.nav-link').contains('Products').click()
      cy.get('a').contains('Add a product').click()
      cy.get('#newAllergen').children('input').type('test').parent().children('matbutton').click()
      
      cy.get('matlistItem').children('div').contains('test').parent().children().children('matbutton').contains('Remove').should('have.length', 1)
      cy.get('matlistItem').children('div').contains('test').parent().children().children('matbutton').contains('Add').click()
      cy.get('#Name').type('testProduct')
      cy.get('input').contains('Create').click()

      cy.get('.nav-link').contains('Products').click()
      cy.get('tr').children('td').contains('testProduct').parent().children('td').contains('Delete').click()
      cy.get('input').contains('Delete').click()
    })

    it('Removes allergen, one should be removed, not all of them', () => {
      cy.get('.nav-link').contains('Products').click()
      cy.get('a').contains('Add a product').click()
      cy.get('matlistItem').children('div').contains('Milk').parent().children().children('matbutton').contains('Add').click()
      cy.get('matlistItem').children('div').contains('Nuts').parent().children().children('matbutton').contains('Add').click()
      cy.get('matbutton').contains('Remove').click()
      cy.get('matbutton:contains("Remove")').should('have.length', 1)
      cy.get('#Name').type('testProduct')
      cy.get('input').contains('Create').click()

      cy.get('.nav-link').contains('Products').click()
      cy.get('tr').children('td').contains('testProduct').parent().children('td').contains('Delete').click()
      cy.get('input').contains('Delete').click()
    })

    it('Create allergen, different allergens should appear', () => {
      cy.get('.nav-link').contains('Products').click()
      cy.get('a').contains('Add a product').click()
      cy.get('matlistItem').children('div').contains('Milk').parent().children().children('matbutton').contains('Add').click()
      cy.get('matlistItem').children('div').contains('Nuts').parent().children().children('matbutton').contains('Add').click()
      cy.get('#Name').type('testProduct')
      cy.get('input').contains('Create').click()

      cy.get('.nav-link').contains('Products').click()
      cy.get('tr').children('td').contains('testProduct').parent().children('td').contains('Details').click()
      cy.get('div:contains("Milk")').children('a:contains("I\'m allergic to this")').should('have.length.greaterThan', 0)
      cy.get('div:contains("Nuts")').children('a:contains("I\'m allergic to this")').should('have.length.greaterThan', 0)
      
      cy.get('.nav-link').contains('Products').click()
      cy.get('tr').children('td').contains('testProduct').parent().children('td').contains('Delete').click()
      cy.get('input').contains('Delete').click()
    })
  })
  