export const useSeason = () => useState<Number>('season', () => 6)
export const usePlayoffs = () => useState<Boolean>('playoffs', () => false)