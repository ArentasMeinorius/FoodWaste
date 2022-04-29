---
papersize:
- a4
fontsize:
- 12pt
geometry:
- margin=1in
fontfamily:
- charter
header-includes:
- \setlength\parindent{24pt}
---

\setcounter{page}{1}
\pagenumbering{arabic}
\begin{titlepage}
   \begin{center}
       \vspace*{1cm}

       \textbf{Food Waste}

       \vspace{1.5cm}

       \textbf{Arentas Meinorius,\\Jaunius Tamulevičius,\\Martinas Mačernius,\\Pijus Petkevičius}

       \vfill

       \vspace{0.8cm}

       Matematikos ir informatikos fakultetas\\
       Vilniaus universitetas\\
       Lietuva\\
       \today

   \end{center}
\end{titlepage}

# Summary {.unlisted .unnumbered}
&nbsp;&nbsp;&nbsp;&nbsp;The primary objective of the second laboratory assignment is to design the system and required changes. While in the first laboratory work we analysed business and all its processes, this time the attention on existing system and the changes. \

**The main tasks of this iteration:** \
   1. Domain model. \
   2. Use cases. \
   3. Sketches. \
\clearpage
\tableofcontents
\clearpage

# Context
For a system to be successful, it must be developed with the intention of solving a real-world problem, which, in our case, is reducing food waste in restaurants and shops. The software is useless if it does not solve required problem. In this part we analyse our problem and how it is intended to be solved. 

## Goal of the system
Reduce food waste by distributing it.

### The problem
Not all food products are sold before spoiling, sometimes restaurants do not use all the food they have bought.

### Solution
Prepare a plaftorm that would stand as a middle man helping people sell excess food while  allowing others to buy it cheaper.

## Planned changes
To further develop and increase the functionality of the existing system we created several tasks:

User registers to the platform and creates Restaurant account. User can have multiple restaurants (restaurant group). They can be added or removed when the user creates or edits profile data. When food product is added for the restaurant, user can choose one or more restaurants.

Every food Product has an allergens list. The list can be changed when the product is added or modified.

User can have Reservations of food products. Payment information is provided with the reservation.

User can have its favourite restaurants. In the page of a physical restaurant the user subscribes to the restaurant and its added to the favourite retaurans listm. The notifications are sent whenever the new products are added to the FoodWaste system favourite restaurant. User can also get notifications when an individual product is favoured.

### Change list
 + Restaurant might have multiple addresses.\
 + Remove reservation.\
 + Allergens tags on food products.\
 + Notifications for selected restaurants (whenever they add a new product)

\clearpage
### Impact of changes

### Domain model

![Domain model](Assets/lab3/DomainModel.png "Domain model")

### Glossary

### UI sketches

### Use case models

1. Multiple addresses for restaurant.

Main scenario (adding new address):
Administrator of the restaurant presses account bubble in the top right corner of the main screen. Multiple options show up in a list. Administrator presses “Account settings”. Settings screen contains “Addresses” subsection. User clicks green “Add new address” button. User inputs the address and presses save. System calls Google API to find the address. If the address is found - green “Address added” modal shows up in top right corner. User profile shows multiple addresses (including the new one) under “Addresses” subsection.

Alternative scenario:
If the address is not found on google maps – red “Address not found on google maps” modal shows up in the top right corner. User profile shows old addresses under “Addresses” subsection.


Main scenario (removing one of the addresses):
Administrator navigates to “Addresses” subsection in profile settings (as described in adding new address scenario). User sees red crosses near each address. Hovering over red cross displays a tooltip “Remove this address". User presses the button. Confirmation modal shows up with "This will remove all listings of this address”. User proceeds with deletion. Address is removed as well as all listings associated with it. Green modal shows up in top right corner signaling “Address removed successfully”

Alternative scenario:
Address is the last one – red cross is grayed out. Hovering over the button displays tooltip “Cannot remove the last address”.

![Use case diagram](Assets/lab3/useCase1_newAddress.png "Use case diagram")

2. Removing reservations
Main scenario:
User clicks on “My reservations”. New page with the list of reservations is loaded. Red crosses are displayed over each one. Hovering over red cross displays tooltip “You may cancel this reservation”. User clicks the button. Modal warning window pops up with the text “You are about to remove reservation. Doing this more than once in 24hours may result in negative reputation (keep in mind, that not arriving on your reservation *will* result in negative reputation). You may read about this more in our F.A.Q (link). Do you want to remove reservation?”.  User presses cancel and same reservations page is displayed.

Alternative scenario:
User confirms reservation removal. Reservation is removed from the list. In the top right corner green modal pop up displays “Reservation removed successfully”. System displays reservations page without canceled reservation.

![Use case diagram](Assets/lab3/useCase2_removeReservation.png "Use case diagram")

3.Allergen list for sold products

Main scenario: The user clicks on the profile tab. The user clicks the allergen subsection. The user selects allergens from a given list. The user clicks on „Save changes“ button. The user clicks on the restaurant tab. The user selects a restaurant. The user selects a product that has component that he has selected as his allergen. The system displays a warning box at the products description about the allergen.

Alternative scenario 1: The user clicks the button “Add to cart” button to add the product with his allergen. The system shows a pop-up dialogue warning the user that he wants to add a product that he is allergic to his cart. The user clicks “Do not put this in the cart” button. The system does not add the product to the user’s cart.

Alternative scenario 2: Unregistered user clicks on the restaurant tabs. The user selects a restaurant. The user clicks on a product. The user clicks on a tab named “Allergens”. The user checks if the list that is shown in the tab. The user finds an allergen that affects him. The user does not add the product in his cart.

Diagram:

![Allergen information](Assets/UseCaseDiagram3.png "Allergen information")

4. Notifications for chosen restaurants

Main scenario: In the page of a physical restaurant the user presses a crimson button with white text, saying "Favourite". The system adds this physical restaurant to the users list of favoured restaurants. The system reports successful subscription by making the button become white with ruby outline and text, saying "Favoured". The system shows the restaurant in the "Restaurant notifications" tab in "Favoured restaurants" page. The restaurant updates their food product information. The system sends the user a notification about the update. The user clicks "Restaurant notifications" tab to see updates from all their favoured restaurants or clicks on "Favoured restaurants" in this tab to see the list with their favoured restaurants that have been updated since last time they checked. Clicking a specific restaurant in "Favoured restaurants" page the user sees what exact products were added.

Alternative scenario: The user presses the “Restaurants” tab. The user clicks on a specific physical restaurant. The user presses a dark lilac button saying "Favourite". The system adds this product from this physical restaurant to the users list of favoured products. The system reports successful subscription by making the button become white with dark lavender outline and text, saying "Favoured". When this specific product can be purchased from this store the user is notified.

Diagram:

![Restaurant notifications](Assets/UseCase4.png "Restaurant notifications")


\clearpage

### GUI Sketches

#### Req. 1 - Several addresses for a restaurant

![Several restaurant addresses sketch](Assets/lab3/sketch_address.png)

\clearpage

#### Req. 2 - Cancel reservation

![Cancel product reservation sketch](Assets/lab3/sketch_removeRes.png)

\clearpage

#### Req. 3 - Alergen addition and warnings

![User allergens sketch p1](Assets/lab3/allergens_1.jpg)

![User allergens sketch p2](Assets/lab3/allergens_2.jpg)

\clearpage

![Product allergens](Assets/UseCaseSketch.png)

![Product purchase warning](Assets/UseCase3SketchWithAWarning.png)

\clearpage

#### Req. 4 - New product notification

![Newly added product notification](Assets/lab3/notifications.jpg)