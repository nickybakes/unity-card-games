# All In-teractive Video Card Games
All code and graphics made by Nick Baker. No AI was used.

"Delmon Delicate" font made by Atharuah Studios and used under a personal license.

Note: Skip the intro animation in Editor
-
The intro animation can be automatically skipped in the Editor by checking this box on the Title Intro Canvas object in SCE_AppInit_01 scene.

<img width="326" height="137" alt="image" src="https://github.com/user-attachments/assets/7e50511a-8dc8-43bf-8d8c-4e7306a2a6d1" />

# Game Content Creation
For this project I made use of scriptable objects and custom inspectors to make card game creation and iteration very fast for designers. You can find more of these in the Content folder in the Unity project.
Here are some examples:

Poker Hands
-
I made a system of combining basic hand components to form rules that when combined, form poker hands. This means that poker hands are not hard coded, and new ones can be easily created by a game designer without touching any code!

<img width="1621" height="564" alt="image" src="https://github.com/user-attachments/assets/17c879ea-f7de-4236-81cc-a90eff4dadaf" />

These poker hands are then referenced in the paytable, and the game will compare the player's hand to each Poker Hand to find the highest Poker Hand on the paytable that the hand contains.

<img width="538" height="439" alt="image" src="https://github.com/user-attachments/assets/c20ce8ca-34ed-4a11-88e5-03c52599c1c1" />

Game Rules
-
The values that drive the rules of a gamemode are also done through scriptable objects. So making variants of games (such as all the different game choices in Game King) would be quick.

<img width="1081" height="455" alt="image" src="https://github.com/user-attachments/assets/bd44ad8b-8bd2-471a-9e31-bfaa97488a7e" />

Decks
-
If a game needs a specific sized/customized deck, that is also easy to do. Games load Deck data from highly customizable scriptable objects.

<img width="1600" height="600" alt="image" src="https://github.com/user-attachments/assets/dc07a6bd-f589-4d2d-aec0-70cd85db3bf7" />
