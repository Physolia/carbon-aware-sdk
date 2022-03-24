# Carbon Aware REST API

## Endpoints

### POST /sci-scores

This endpoint uses a custom plugin which calculates the SCI score using the Green Software Foundation SCI specification formula.

> ((E * I) + M)/R

- (E) Energy
- (I) Marginal Carbon Intensity
- (M) Embodied Emissions
- (R) Functional Unit

The request object is defined by the plugin, but the response object MUST include the SCI score and the component variables.  EG

```
{
  "sciScore": 80.0,
  "energyValue": 1.0,
  "marginalCarbonIntensityValue": 750.0,
  "embodiedEmissionsValue": 50.0,
  "functionalUnitValue": 10
}
```