# All In-teractive Video Card Games
All code and graphics made by Nick Baker. No AI was used.

"Delmon Delicate" font made by Atharuah Studios and used under a personal license.

Note: Skip the intro animation in Editor
-
The intro animation can be automatically skipped in the Editor by checking this box on the Title Intro Canvas object in the SCE_AppInit_01 scene.

<img width="326" height="137" alt="image" src="https://github.com/user-attachments/assets/7e50511a-8dc8-43bf-8d8c-4e7306a2a6d1" />

# Game Content Creation
For this project I made use of scriptable objects and custom inspectors to make card game creation and iteration very fast for designers. You can find these in the Content folder in the Unity project.
Here are some examples:

Poker Hands
-
I made a system of combining basic hand components to form rules that form poker hands. This means that poker hands are not hard coded, and new ones can be easily created by a game designer without touching any code!

<img width="1621" height="564" alt="image" src="https://github.com/user-attachments/assets/17c879ea-f7de-4236-81cc-a90eff4dadaf" />

Paytables
-
These poker hands are then referenced in the paytable, and the game will compare the player's hand to each Poker Hand to find the highest Poker Hand on the paytable that the player's hand contains.

<img width="538" height="439" alt="image" src="https://github.com/user-attachments/assets/c20ce8ca-34ed-4a11-88e5-03c52599c1c1" />

Game Rules
-
The values that drive the rules of a gamemode are also done through scriptable objects. So making variants of games (such as all the different game choices in Game King) would be quick.

<img width="1081" height="455" alt="image" src="https://github.com/user-attachments/assets/bd44ad8b-8bd2-471a-9e31-bfaa97488a7e" />

Decks
-
If a game needs a specific sized/customized deck, that is also easy to do. Games load Deck data from highly customizable scriptable objects.

<img width="1600" height="600" alt="image" src="https://github.com/user-attachments/assets/dc07a6bd-f589-4d2d-aec0-70cd85db3bf7" />


# Game Architecture
The development side of the game is split by Backend and Frontend (basically MVC). 

The Backend (Game Manager) handles loading game data, the scripting of a gamemode, score evaluation, etc. It outputs data that describes the current Game State and how its changing.

The Frontend (Game View Manager) parses that Game State data in order to control the game elements on screen.

The Backend does not care about how a Card moves on screen, it just knows specific Cards were drawn from specific Decks to specific Hands. The Frontend never tries to calculate score or directly change game data, it only cares about making sure the objects on screen match the state the Backend gave it, and that they look as cool as the artist intended. 

The two sides just need to be set up in the scene so that they can talk to eachother. When the player clicks the "Hit" button in Blackjack, the Frontend knows a button has been clicked, but doesn't know what its supposed to do, it just tells the Backend that the player clicked a button with a specific index identifier. The Backend can then understand that and draw a card, check if the player busted, etc, and the Frontend will then display those changes.

If you put the Backend on a remote server to ensure no player tampers/cheats, then the Frontend would just display the game and never try to change data.

Modularity
-
Because of this separation of powers, the system is very modular. You can see this in the first second that you open the game: the Intro Animation is not hand animated, but rather is just drawing some cards and sending that data to the Game View Manager, which then displays them the same way as in a normal game.

The Frontend elements are also designed to be easily customized by artists. Most things are driven by artist defined animations, shader materials, or editable values.

The timings and movement of Cards on screen can be customized with a simple Animation Curve. In this project I made the Cards slightly overshoot their goal, which I thought felt pretty good. But that can be very easily changed.

<img width="573" height="439" alt="image" src="https://github.com/user-attachments/assets/0185f8b3-a5d4-4150-9d2f-feb4dd39ae13" />


When Cards are positioned in a Hand, they sit along a customizable spline track in the Hand Display. Card spacing values can also be adjusted in the inspector for the Hand Display.

<img width="936" height="390" alt="image" src="https://github.com/user-attachments/assets/90a5d4bc-bd22-4eae-b247-ccc1c366a325" />


Even the individual elements on a Card can be customized per Suit by using a Card Visual Profile scriptable object.

<img width="542" height="439" alt="image" src="https://github.com/user-attachments/assets/232e367f-9389-433a-85cd-24b62e697b8c" />


I also develop my shaders to be extremely customizable. This is the material for the Scrolling Suits that you see in the backgrounds and on the UI buttons.

<img width="393" height="889" alt="image" src="https://github.com/user-attachments/assets/45026595-3b08-4624-a98b-d2c5e44fb4d1" />


Singletons
-
I use the singleton pattern for objects in the game that are global for the entire program. They are set to not be auto-destroyed by Unity when the scene changes. For this project, there are three:

- AppManager: the first object loaded, handles the state of the App and scene management.
- UserManager: handles User data such as their balance, selected bet, and market/location info. This would be where logging into a User's account and retrieving that data would be done.
- GameCollectionManager: handles storing all the game choices that the player can pick from to play.

I used the Singleton pattern for these as I view them as administrators on the overall program. The game would not be playable without them existing, and there should only ever be one of them, so the game always forces you to first enter the SCE_AppInit_01 scene on startup to make sure they are properly initialized.

If needed to, these could be changed to not be singletons.
