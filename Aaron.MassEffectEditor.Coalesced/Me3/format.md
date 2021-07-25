# Mass Effect 3 Coalesced.bin File Format

Coalesced.bin stores a collection of files used to configure Mass Effect 3. It can be thought of as a binary archive of .ini files.

When decoded, the results look like this:

* Each Coalesced.bin contains multiple Files
* Each File contains mulitple Sections.
* Each Section contains mulitple Entries.
* Each Entry contains multiple values.
* Each value is a string

## Header

The binary file starts with a header that is used to decode the rest of the file.

HeaderLength is 8 * 4 = 32 bytes long.

The Header Block is laid out like this (all offsets are from the start of the file):

| Offset | Name              | Type | Description
|--------|-------------------|------|-------------------------------------------------------------------------------
| 0x0000 | Magic Word        | uint | Always: 1718448749 AKA "mrmf".
| 0x0004 | Version           | uint | Always: 1. Prehaps there are other versions, but I've never seen them.
| 0x0008 | MaxKeyLength      | int  | The length of the longest string in the String Table Block.
| 0x000C | MaxValueLength    | int  | The length of the longest string that has been compressed in the Data Block
| 0x0010 | StringTableLength | uint | The size of the String Table Block in bytes.
| 0x0014 | HuffmanLength     | uint | The size of the Huffman Tree  Block in bytes.
| 0x0018 | IndexLength       | uint | The size of the Index Block in bytes.
| 0x001C | DataLength        | uint | The size of the Data Block in bytes.

The whole file is laid out like this:

| Size              | Name               | Description
|-------------------|--------------------|---------------------------------------------------------------------------
| HeaderLength      | Header Block       | A simple structure containing values used to decode the other blocks.
| StringTableLength | String Table Block | A list of strings and checksum values.
| HuffmanLength     | Huffman Tree Block | A list of pair values used to reconstrucut the Huffman tree used to compress the Data
| IndexLength       | Index Block        | A table used to map the files, sections, entries and values to each other.
| 4 (uint)          | Compressed Size    | The number of bits in the Data Block
| DataLength        | Data Block         | A null delimited list of strings that has been compressed using [Huffman encoding](https://en.wikipedia.org/wiki/Huffman_coding)

## String Table

The string table is located at: HeaderLength.

It is divided into 3 sections:
* Header
* Index
* Content

The String Table Block Header is laid out like this (all offsets are from the start of the block):

| Offset | Name              | Type | Description
|--------|-------------------|------|-------------------------------------------------------------------------------
| 0x0000 | StringTableLength | uint | This is a reducnted value that should be the same as the value in the header.
| 0x0004 | EntryCount        | uint | The number of entries in the string table

