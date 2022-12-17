A cellular automata recreation of Powder Toy using C# and Raylib.

![](Media/sand.gif)

TODO:

- Physics
  - Complete physics rules for basic 4 material types - solid, liquid, gas, powder
  - Add external forces such as gravity (done) and wind
  - Change cells' local understanding of direction based on the direction of gravity
  - Do extra pass over physics rules, adding more in-depth features such as density (done) and buoyancy
  - Add explosions
  - Add fire as a 5th material type (?)
  - Add chemical reactions if I end up having enough materials to justify it

- Materials
  - Solids: Stone, Wood
  - Liquids: Water, Lava, Acid
  - Gases: Steam, Smoke, Toxic Gas
  - Powders: Sand (done), Salt, Dirt, Explosive

- Optimisation
  - Add cell chunking
  - Make chunks sleep when calculations are not necessary

- Extra
  - Add procedural terrain generation using noise to create starting environments
