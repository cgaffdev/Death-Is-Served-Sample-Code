# Death Is Served: Sample Code

**WARNING!** You are about to look at *SPAGHETTI CODE!!!* ⚡⚡⚡😱😱😱

Death is Served was a short game made in just 5 DAYS for a spooky game jam in 2025. With such a short amount of time, and with me being the sole programmer and Unity developer, I had a lot to do. Tasks and their problems had to be tackled on the fly.

The sample code consist of a good handful of the minigames such as the stovetop, the oven, stirring a pot, picking bones, and cutting up flesh!

If I had more time and the chance to improve upon this game, here are some of the broad changes I would make:
- **A Universal Sprite Grabber:** The issue with the system is that there are two core sprite grabbers, the main difference being that one is a physics-based grabber. With more time to plan, I would've made a single, universa physics-based grabber.

- **Keep the Mouse Locked onto Sprites:** With a physics-based sprite grabber, if an object gets stuck on a collider, the mouse can move away from the sprite while technically still grabbing it. This separation is temporary, lasting until the sprite is freed. I would have preferred to lock the mouse onto the sprite so that both would have to get unstuck together rather than separately.

- **More Cohesive Code:** About half of the comments you can see in my code were added in the purpose of uploading it to github. Many of the functions and variables could have been named better to further entail what they do.

- **Universal Item Adder:** Rather than each minigame directly calling seperate functions to the player to reward, I would have preferred a streamlined system to give the player a reward instead of numerous functions. 

- **Abstract Minigame Class:** All the minigames have have things in common, I wish I had created a base abstract class to handle their shared functionarilities.

## About the game
“Death is Served” is a small Indie game created in five days.
The team included two artists and myself as the sole developer. 

Set in the Land of The Dead, a witch (the player) finds themselves needing an escape. Equipped with skills from a past life, she takes a job in the afterlife, turning death into a new opportunity: a job opportunity!



The core gameplay loop centers around time-based order management and interactive minigames. The shift begins with a short grace period, allowing our witch to prepare raw ingredients ahead of time. Orders are communicated through the notepad and emoji bubbles. Recipes are made to be intuitive, guiding you through the steps you need to take by reading the order's name in reverse.

### Check out the game for yourself: https://tinup.itch.io/death-is-served
