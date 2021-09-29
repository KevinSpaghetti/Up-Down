# Up&Down

A simple endless runner game developed for the course of video game programming at the University of Udine.

![The main menu](./Screenshots/MainMenu.png)

## Theme
The theme for this game was Up&Down so the car can avoid obstacles by going left, right, or by switching up and down.

![The gameplay](./Screenshots/StillGameplay.png)


## Optimizations
Since there can be a lot of objects on the screen at once the game takes advantage of object pooling, instancing and batching to reduce the GPU load.