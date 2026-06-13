import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { DataTable } from "@/components/ui/data-table"
import { columns } from "@/components/records-columns"
import { Skeleton } from "@/components/ui/skeleton"
import { Input } from "@/components/ui/input"
import NotFound from "@/pages/not-found"
import TopNavActions from "@/components/ui/top-nav-actions"

import { record, domain } from "@/lib/api"
import { useDebounce } from "@/lib/useDebounce"
import DialogAdd from "@/features/DialogAddContainer"

import { useEffect , useState } from "react"
import { toast } from "sonner"
import type { RecordsDto } from "@/types/records"
import { useParams } from "react-router-dom"

function DashboardRecords() {
  const domainId = parseInt(useParams<{ domainId: string }>().domainId)
  const [recordsData, setRecordsData] = useState<RecordsDto[]>([])
  const [error, setError] = useState<string | null>(null)
  const [searchQuery, setSearchQuery] = useState("")

  const searchQueryDebounced = useDebounce(searchQuery, 500)

  useEffect(() => {
    if (isNaN(domainId)) {
      toast.error("Invalid domain ID")
      // eslint-disable-next-line react-hooks/set-state-in-effect
      setError("Invalid domain ID")
      return
    }

    const checkDomainExists = async () => {
      try {
        await domain.getById(domainId)
        loadData()
      // eslint-disable-next-line @typescript-eslint/no-unused-vars
      } catch (error) {
        toast.error("Domain not found")
        setError("Domain not found")
      }
    }

    const loadData = async () => {
      try {
        const data = await record.getAll(domainId)
        setRecordsData(data)
      // eslint-disable-next-line @typescript-eslint/no-unused-vars
      } catch (error) {
        toast.error("Failed to load records")
        setError("Failed to load records")
      }
    }

    checkDomainExists()
  }, [domainId])

  if (error) {
    return <NotFound />
  }

  return (
    <>
      <TopNavActions />
      <Card className="w-[25vw]">
        <CardHeader className="flex flex-row items-center justify-between">
          <div>
            <CardTitle className="text-left">Dashboard</CardTitle>
            <CardDescription>
              Overview of your records.
            </CardDescription>
          </div>
          <Input
            className="w-1/2"
            id="feature-2"
            placeholder="Search Records..."
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </CardHeader>
        <CardContent>
          <section id="feature-1">
            {recordsData.length === 0 ? (
              <Skeleton className="w-full h-[50vh]" />
            ) : (
              <DataTable columns={columns} data={recordsData} searchQuery={searchQueryDebounced} searchColumn="recordName" />
            )}
            <DialogAdd domainId={domainId} />
          </section>
        </CardContent>
      </Card>
    </>
  )
}

export default DashboardRecords