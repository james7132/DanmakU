#Contributing to HouraiLib

There aren't many requirements to contribute to this project:

0. Make sure the code works. Any pull request will be tested before merging.
0. Ensure non-code assets included are serialized in text format. For example, binary serialized prefabs will be rejected.
0. Prefabs that reference non-present assets are to be removed.

This repository is structured to make it easy to include the project in any Unity project via git subtree. The suggested method
for contributing is as follows:

0. [Fork](https://github.com/HouraiTeahouse/HouraiLib/new/master#fork-destination-box) this project.
0. (Optional) Use git subtree to include the code in another project.
0. Make your changes.
0. Test your changes.
0. If you included it as a subtree, push your changes to your fork.
0. Start a pull request.
