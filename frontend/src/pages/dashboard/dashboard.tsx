import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { DataTable } from "@/components/ui/data-table"
import { columns } from "@/components/domains-columns"
import { Skeleton } from "@/components/ui/skeleton"
import { Input } from "@/components/ui/input"
import TopNavActions from "@/components/ui/top-nav-actions"

import { domain } from "@/lib/api"
import { useDebounce } from "@/lib/useDebounce"
import DialogAdd from "@/features/DialogAddContainer"

import { useEffect , useState } from "react"
import { toast } from "sonner"

function Dashboard() {
  const [dnsData, setDnsData] = useState([])
  const [searchQuery, setSearchQuery] = useState("")
  const searchQueryDebounced = useDebounce(searchQuery, 500)

  async function fetchData(): Promise<void> {
    try {
      const data = await domain.getAll()
      setDnsData(data)
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    } catch (error) {
      toast.error("Failed to fetch DNS data")
    }
  }

  useEffect(() => {
    fetchData()
  }, [])

  return (
    <>
      <TopNavActions />
      <Card className="w-[75vw]" id="welcome-title">
        <CardHeader className="flex flex-row items-center justify-between">
          <div>
            <CardTitle className="text-left">Dashboard</CardTitle>
            <CardDescription>
              Overview of your DNS.
            </CardDescription>
          </div>
          <Input
            className="w-1/5"
            id="feature-2"
            placeholder="Search DNS Records..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </CardHeader>
        <CardContent>
          <section id="feature-1">
            {dnsData.length === 0 ? (
              <Skeleton className="w-full h-[50vh]" />
            ) : (
              <DataTable columns={columns(fetchData)} data={dnsData} searchQuery={searchQueryDebounced} />
            )}
          </section>
          <DialogAdd refreshCallback={fetchData} />
        </CardContent>
      </Card>
    </>
  )
}

export default Dashboard