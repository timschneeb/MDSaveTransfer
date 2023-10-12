 

# Muse Dash save data tools

## Features

The main feature of this app is save file conversion:

* Convert a Nintendo Switch save file to a PC save file

  * The Switch version does not support the official account sync and also uses a completely different save data format.

This app also contains tools that allow inspection and modification of Muse Dash's binary save file format of the PC version:

* Convert a PC save file into a human-readable JSON file, while preserving the exact file structure.

* Convert a JSON file (generated using this app) back to a PC save file
  * This allows you to manually edit save files.
  * Note: The generated JSON file is very verbose and contains a lot of C# type references. These are required to safely convert the JSON back to Muse Dash's binary save file format.

## Usage

The WebAssembly application is available here: https://md.timschneeberger.me

All the processing happens locally in your browser. No save files are uploaded.

## Roadmap

* Planned: ability to combine save data from the Nintendo Switch and PC version.
  * Automatically pick the best scores for each stage and combine collected items, unlocked stages, and more from the two saves into one.

