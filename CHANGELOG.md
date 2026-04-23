# Changelog

All notable changes to this project will be documented in this file.

The format is based on Keep a Changelog, and this project follows Semantic Versioning.

## [Unreleased]

## [0.4.0] - 2026-04-23

- Refactored holiday calculation internals toward a more functional style using immutable holiday definitions and projection-based pipelines.
- Added targeted regression tests for alias lookup, observed-date carryover, and upcoming holiday ordering.
- Expanded the sample console app to demonstrate supported-name discovery, alias catalogs, and `TryGet...` lookups.

## [0.3.0] - 2026-04-23

- Added a dedicated `HolidayAliases` catalog with grouped federal and religious alias collections.
- Updated holiday lookup internals and tests to use shared alias catalogs and holiday-name constants consistently.

## [0.2.0] - 2026-04-23

- Added `HolidayDateMode` overloads for upcoming holiday queries so callers can search by actual or observed date.
- Added a sample console application demonstrating lookup and upcoming holiday APIs.
- Added non-throwing `TryGet...` holiday lookup APIs.
- Added APIs to list supported holiday names and accepted lookup aliases.

## [0.1.0] - 2026-04-21

### Added

- Federal holiday calculation support with historical US observance transitions
- Religious holiday support for Easter-derived observances
- Single-holiday and collection-based lookup APIs
- Upcoming holiday APIs for federal and religious holidays
- XML documentation across the public API surface
- GitHub Actions CI for restore, build, test, and package validation
- NuGet package metadata and README packaging support
