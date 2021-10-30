# Shader Basics, Blending & Textures • Shaders for Game Devs [Part 1]

[series by Freya Holmér](https://www.youtube.com/watch?v=kfM-yu0iQBk&t=8065s)

These structures & syntax are based on Unity's shader system, and may have differences with other shader systems.

# Structure of a Shader

* `.shader`
  * Properties- input data
    * often defined with materials which contain data about:
      * colors
      * values
      * textures
      * materials also contain references to the shader that will be used to render it  
    * indirect properties- implicitly passed in
      * mesh
      * matrix
  * SubShader- can have multiple subshaders in shader file. Can be used for optimization
    * Pass- render pass/draw (Can contain multiple passes)
      * **vertex shader**- written in HLSL (High level shader language). Everything else in Unity is written in ShaderLab
      * **fragment shader**- written in HLSL 

## Vertex Shader

* Like a `foreach` loop that goes through each vertex in the mesh and runs shader code
  * object placement in "clip space"
    * clip space: normalized space from (-1, 1) inside view
    * take local space coordinates of all of the vertices and transform using MVP (Model View Projection) matrix to convert to clip space
    * can modify vertices if desired
* often used for animating water, swaying leaves, because you can use vertex shader to make things move
* after vertex shader rasterization step occurs before beginning Fragment Shader
* data can be sent from vertex shader to the fragment shader, but not vice versa
  * this is what the `Interpolator` in the code is used for
    * this is often used to interpolate values of data between vertices
      * data can be anything. color, normals, etc. need to have a smooth transmission between vertices
      * Barycentric Interpolation
* because there are usually more fragments than vertices, a common method of optimization is to put calculations in the Vertex Shader, to minimize their amount

## Fragment Shader

* Like a `foreach` loop over every "fragment"
  * fragment: because at this stage the pixels don't necessarily correspond to the pixels on your screen, the term fragment is used to avoid confusion
  * determines fragment colors